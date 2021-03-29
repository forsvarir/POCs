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

@Controller()
public class ChatController {
    static int connectionId = 0;

    @Autowired
    SimpMessagingTemplate messagingTemplate;

    @MessageMapping("/connect")
    @SendToUser("/queue/reply")
    public StatusMessage connect(ConnectMessage message) throws Exception {
        return new StatusMessage(connectionId++);
    }

    @MessageMapping("/message")
//    @SendTo("/topic/greetings")
    public void greeting(HelloMessage message) throws Exception {
        if (message.getMessage().contains(":")) {
            var tokens = message.getMessage().split(":");
            final String userTargetAddress = "/queue/" + tokens[0];
            messagingTemplate.convertAndSend(userTargetAddress, new Greeting(HtmlUtils.htmlEscape(message.getMessage())));
        } else {
            messagingTemplate.convertAndSend("/topic/greetings", new Greeting(HtmlUtils.htmlEscape(message.getMessage())));
        }
    }
}