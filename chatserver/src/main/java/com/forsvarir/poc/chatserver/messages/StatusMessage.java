package com.forsvarir.poc.chatserver.messages;

public class StatusMessage {

    private final int connectionId;

    public StatusMessage(int connectionId) {
        this.connectionId = connectionId;
    }

    public int getConnectionId() {
        return connectionId;
    }
}
