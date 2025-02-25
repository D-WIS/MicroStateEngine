# Microstate Threshold Server
The configuration file contains the thresholds for the microstates. The ThresholdServer reads the configuration regularly. If there
is a difference since last time it reloads all the thresholds and publishes them on the Blackboard.

## Getting started
The `docker run` command for windows is:
```
docker run -d --name thresholdserver -v C:\Volumes\DWISMicroStatesThresholdsServer:/home digiwells/dwismicrostatethresholdsserver:stable
```
where `C:\Volumes\DWISMicroStatesThresholdsServer` is any folder where you would like to access the config.json file that is used to configure
the application.

and the `docker run` command for linux is:
```
docker run -d --name thresholdserver -v /home/Volumes/DWISMicroStatesThresholdsServer:/home digiwells/dwismicrostatethresholdsserver:stable
```
where `/home/Volumes/DWISMicroStatesThresholdsServer` is any directory where you would like to access the config.json file that is used to
configure the application.

## Getting started (internal)
If you have created the docker image yourself, here is the procedured.

The `docker run` command for windows is:
```
docker run -d --name thresholdserver -v C:\Volumes\DWISMicroStatesThresholdsServer:/home dwismicrostatethresholdsserver:latest
```
where `C:\Volumes\DWISMicroStatesThresholdsServer` is any folder where you would like to access the config.json file that is used to configure
the application.

and the `docker run` command for linux is:
```
docker run -d --name thresholdserver -v /home/Volumes/DWISMicroStatesThresholdsServer:/home dwismicrostatethresholdsserver:latest
```
where `/home/Volumes/DWISMicroStatesThresholdsServer` is any directory where you would like to access the config.json file that is used to
configure the application.
