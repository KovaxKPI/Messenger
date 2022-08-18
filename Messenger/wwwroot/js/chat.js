"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/Chat").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message, to) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

connection.on("Notify", function (message) {

    // создает элемент <p> для сообщения пользователя
    let li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);

    li.textContent = `${message}`;

});
/*connection.on("ReceiveMessagePrivate", function (user, message, to) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message} to ${to}`;
});*/

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    var rec = document.getElementById("receiver").value;
    connection.invoke("SendMessage", user, message, rec).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});