"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/Chat").build();

document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${user} says ${message}`;
});

connection.on("Notify", function (message) {

    let li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);

    li.textContent = `${message}`;

});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    connection.invoke("AddToGroups");
}).catch(function (err) {
    return console.error(err.toString());
    
});

document.getElementById("connectButton").addEventListener("click", function (e) {
    var userName = document.getElementById("userInput").value;
    var userGroup = document.getElementById("roomName").value;
    connection.invoke("ConnectRoom", userName, userGroup);
    e.preventDefault();
});

document.getElementById("disconnectButton").addEventListener("click", function (e) {
    var userName = document.getElementById("userInput").value;
    var userGroup = document.getElementById("roomName").value;
    connection.invoke("DisconnectRoom", userName, userGroup);
    e.preventDefault();
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    var rec = document.getElementById("receiver").value;
    var userGroup = document.getElementById("userGroup").value;
    connection.invoke("SendMessage", user, message, rec, userGroup).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

/*document.getElementById("sendPrivateButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    var rec = document.getElementById("receiver").value;
    var userGroup = document.getElementById("userGroup").value;
    connection.invoke("SendPrivateMessage", user, message, rec, userGroup).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("sendGroupButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    var rec = document.getElementById("receiver").value;
    var userGroup = document.getElementById("userGroup").value;
    connection.invoke("SendMessageGroup", user, message, rec, userGroup).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});*/