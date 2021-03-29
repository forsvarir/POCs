package com.forsvarir.poc.chatserver;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.messaging.simp.SimpMessagingTemplate;
import org.springframework.scheduling.concurrent.ThreadPoolTaskScheduler;
import org.springframework.stereotype.Component;

import javax.annotation.PostConstruct;

@Component
public class HeartbeatScheduler {
    @Autowired
    private ThreadPoolTaskScheduler taskScheduler;

    @Autowired
    private SimpMessagingTemplate messagingTemplate;

    @PostConstruct
    public void scheduleHeartbeatMessage() {
        taskScheduler.scheduleAtFixedRate(new HeartbeatTask("2 second heartbeat", messagingTemplate), 2000);
    }

    class HeartbeatTask implements Runnable {

        private final String message;
        private final SimpMessagingTemplate messagingTemplate;

        public HeartbeatTask(String message, SimpMessagingTemplate messagingTemplate) {
            this.message = message;
            this.messagingTemplate = messagingTemplate;
        }

        @Override
        public void run() {
            System.out.println("Runnable Task with " + message + " on thread " + Thread.currentThread().getName());
            messagingTemplate.convertAndSend("/topic/greetings", new Greeting(message));
        }
    }
}
