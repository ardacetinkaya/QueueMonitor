﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/queueHub").build();
google.charts.load('current', { 'packages': ['corechart'] });

connection.start().then(function () {

}).catch(function (err) {
    return console.error(err.toString());
});


