package com.forsvarir.poc.chatserver;

import com.forsvarir.poc.chatserver.messages.ConnectMessage;
import com.forsvarir.poc.chatserver.messages.Greeting;
import com.forsvarir.poc.chatserver.messages.HelloMessage;
import com.forsvarir.poc.chatserver.messages.StatusMessage;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.messaging.handler.annotation.MessageMapping;
import org.springframework.messaging.simp.SimpMessagingTemplate;
import org.springframework.messaging.simp.annotation.SendToUser;
import org.springframework.stereotype.Controller;
import org.springframework.web.util.HtmlUtils;

import java.security.Principal;
import java.util.concurrent.ConcurrentHashMap;
import java.util.logging.Logger;

@Controller()
public class ChatController {
    static int connectionId = 0;
    static ConcurrentHashMap<String, Integer> sessions = new ConcurrentHashMap<String, Integer>();

    Logger logger = Logger.getLogger(ChatController.class.getName());

    @Autowired
    SimpMessagingTemplate messagingTemplate;

    @MessageMapping("/connect")
    @SendToUser("/queue/reply")
    public StatusMessage connect(ConnectMessage connectMessage, Principal principal) throws Exception {
        int userId = connectionId++;
        sessions.put(connectMessage.getUser(), userId);

        logger.info("User Connected: " + connectMessage.getUser() + " as " + userId);
        logger.info("Principal:" + principal.getName());

        return new StatusMessage(userId);
    }

    @MessageMapping("/message")
    public void greeting(HelloMessage message) throws Exception {
        if (message.getMessage().contains(":")) {
            var tokens = message.getMessage().split(":");
            final String userTargetAddress = "/queue/" + sessions.get(tokens[0]);
            messagingTemplate.convertAndSend(userTargetAddress, new Greeting(HtmlUtils.htmlEscape(message.getMessage())));
        } else {
            messagingTemplate.convertAndSend("/topic/greetings", new Greeting(HtmlUtils.htmlEscape(message.getMessage())));
        }
    }
}