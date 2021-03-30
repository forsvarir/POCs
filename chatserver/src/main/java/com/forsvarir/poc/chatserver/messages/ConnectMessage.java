package com.forsvarir.poc.chatserver.messages;

public class ConnectMessage {
    private String user;

    public ConnectMessage() {
    }

    public ConnectMessage(String user) {
        this.user = user;
    }

    public String getUser() {
        return user;
    }

    public void setUser(String user) {
        this.user = user;
    }

}
