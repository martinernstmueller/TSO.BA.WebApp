var buildingStates;
var buildingStateUpdateTimer;

/* Angular */
var buildingControlApp = angular.module('buildingControlApp', ['ngResource']);

// SignalR
buildingControlHub = $.connection.BuildingControlHub,
$.connection.hub.logging = true,
$.connection.hub.start()
    .done(function () {
        console.log("SignalR says Connected!");

        // ToDo: Change to Clients.Group as Signalr3 works on IIS
        //StartUpdateTimer();
        
        
    })
    .fail(function () {
        alert("Could not Connect!");
    });

$.connection.hub.disconnected(function () {
    if (typeof user === "undefined") {
        return;
    }
    //StopUpdateTimer();
    $('#overlay').appendTo('main');
    console.log("User " + user.UserName + " has Disconnected!");

    setTimeout(function() {
        $.connection.hub.start();
    }, 5000); // Restart connection after 5 seconds.
})

buildingControlApp.controller('buildingControlController', buildingControlController);

function buildingControlController($scope, buildingControlFactory) {
    console.log("in buildingControlController...");
    // here we should update all binded values!
    $scope.factorytest = 0;
    $scope.isLoadingVisible = true;
    $scope.light_on = '/images/light_on.png';
    $scope.light_off = '/images/light_off.png';
    $scope.light_wait = '/images/light_wait.png';
    
    $.extend(buildingControlHub.client, {
        updateBuildingStates: function (argBuildingStates) {
            updateBuildingStatesScopeVars($scope, argBuildingStates);
        },
        showMessageOnClient: function (argMessageText, argMessageType) {
            switch (argMessageType) {
                case "Error":
                    alert(argMessageText);
                    break;
            }
        }
            
       });
    
}


buildingControlApp.factory('buildingControlFactory', ['$resource', '$log',
    function ($resource, $log) {
        console.log("in the factory...");

        return {};
    }]);


function updateBuildingStatesScopeVars($scope, argBuildingStates)
{
    console.log("Update " + argBuildingStates.length + " building states...");
    
    for (var k = 0; k < argBuildingStates.length; k++) {
        var buildingStateExists = false;

        // set the image ToDo: should/can we make this more generic?
        if ((argBuildingStates[k].BuildingStateValue == "On") || (argBuildingStates[k].BuildingStateValue == "Off") || (argBuildingStates[k].BuildingStateValue == "Waiting")) {
            var scopeVarName = argBuildingStates[k].BuildingStateID.toLowerCase() + '_img';
            var imageName = '/images/' + argBuildingStates[k].BuildingStateID.toLowerCase() + '_' + argBuildingStates[k].BuildingStateValue + '.png';
            $scope[scopeVarName] = imageName;
            //console.log('Change the image of ' + scopeVarName + 'to' + imageName);
        }
        else {
            // no image to change -> we set the value of the scope variable directely
            $scope[argBuildingStates[k].BuildingStateID] = argBuildingStates[k].BuildingStateValue;
        }

        $scope.$apply();
    }
}

function UpdateBuildingStatesFromServer()
{
    console.log("Call UpdateBuildingStatesFromServer...");
    buildingControlHub.server.getAllBuildingStatesFromMySite().done(function (states) {
        console.log("Back from getting all states...")
        buildingControlHub.client.updateBuildingStates(states);
    })
}

function StartUpdateTimer() {
    console.log("Start Update Timer...");

    buildingControlUpdateTimer = setInterval(UpdateBuildingStatesFromServer, 1000);
}

function StopUpdateTimer() {
    console.log("Stop Update Timer...");
    clearInterval(buildingControlUpdateTimer);
}

