package com.forsvarir.poc.chatserver;

import org.springframework.messaging.handler.annotation.MessageMapping;
import org.springframework.messaging.handler.annotation.SendTo;
import org.springframework.stereotype.Controller;
import org.springframework.web.util.HtmlUtils;

@Controller()
public class ChatController {

    @MessageMapping("/message")
    @SendTo("/topic/greetings")
    public Greeting greeting(HelloMessage message) throws Exception {
        return new Greeting(HtmlUtils.htmlEscape(message.getMessage()));
    }
}