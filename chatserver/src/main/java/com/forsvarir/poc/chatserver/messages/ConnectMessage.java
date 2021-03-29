package com.forsvarir.poc.chatserver.messages;

public class ConnectMessage {
    private String message;

    public ConnectMessage() {
    }

    public ConnectMessage(String message) {
        this.message = message;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

}
