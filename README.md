# Dirigent
## Overview
Dirigent is an application life cycle management tool controlling a set of applications running on one or multiple networked computers. It runs on .net and Mono platforms, supporting both Windows and Linux operating systems.

It allows launching a given set of applications in given order according to predefined launch plan. The plan specifies what applications to launch, on what computers, in what order and what another apps (dependencies) need to be running and initialized prior starting a given application. The dependencies include both local and remote applications. 

Applications can also be terminated or restarted, either individually or all-at-once. An application that is supposed to run continuously can be automatically restarted after unexpected termination or crash.

The applications are continuously monitored whether they are already initialized and still running. Their status is distributed to all agents on all machines.

All operations can be remotely controlled from any agent, from a separate remote control tool or programatically via a small .net library.

A launch plan can be executed automatically on computer startup. To speedup the startup process of a system comprising multiple interdependent computers, certain applications (not dependent on those on other computers) can be launched even before the connection among computers is estabilished.


A single agent executable can be configured to to run either in local or networked mode, with embedded control GUI or as GUIless background process (daemon), or as a command line control app.



## Usage

On each computer where some apps shall be started, install and setup an agent application.

Define launch plans into the shared config file. Make sure all agents use identical shared configuration file.

Start a master on an arbitraty selected computer. Master may not be necessary if the dirigent is used for managing apps on just a single computer.

In the local configuration of each agent specify the IP address and port of the master, as well as the machineId of respective agent.

### Agent command line arguments

By default the agent executeble works as a command line tool to send commands to agents
 
 agent.exe <command> <arg1> <arg2> etc.
 
Zero exit code is returned on success, positive error code on failure.
 
The following options changes the mode of operation:
 --daemon .... no UI at all, just a log file
 --traygui ... an icon in tray with gui control app accessible from the context menu
 --remotectrlgui ... not agent as such (not directly managing any local apps), just a remote control GUI that monitors the apps and remotely send commands to the agents
 
Another options: 
 --singlemachine .... no network, just single-machine operation (no master needed); forces --traygui automatically.
 --minimized .... start minimized (only with --traygui and --remotecontrolgui)
 --logfile xyz.log ... what log file to use
 --autostartplan <plan_name> ... immediately loads and starts executing an initial plan before the connection to the master is estabilished
 
 


## Architecture

#### Agents and master

Each computer is running an agent process. One of the computers runs a master process. Agents connect to a single master. The master's role is to broadcast messages from agents to all other agents.

Agent manages the processes running locally on the same machine where the agent is running. Agent takes care of local application launching, killing, restarting and status monitoring. 

Agents listens to and executes application management commands from master.

Agents publish the status of local applications to master which in turn spreads it to all other agents. The status include whether the app is running, whether it is already initialized etc.

All agents share the same configuration of launch plans - each one knows what applications the others are supposed to run.


#### Launch plan

Launch plan comprises just a list of apps to be launched in given order. At most one plan at a time is active.

Each app in the launch plan has the following attributes:

 - unique text id of the application; togeher with the machine id it makes a unique id
 - application binary file full path
 - startup directory
 - command line arguments
 - the launch order in case of same priority of multiple apps
 - whether to automatically restart the app after crash
 - what computer to launch the application on (unique machine id as text string)
 - what apps is this one dependent on, ie. what apps have to be launched and fully initalized before this one can be started
 - a mechanism to detect that the app is fully initialized (by time, by a global mutex, by exit code etc.)
 
#### Templated launch plan definition

Plan definition in an XML file uses a template sections allowing the inheritance of attributes. Every record in the plan can reference a template record. All the attributes are loaded first from the template and only then they can ge overwritten by equally named attributes from the referencing entry. A template record can reference another more generic template record.

#### Computer list

For each computer there is a textual machine id and the IP address defined. One of the machines is marked as master. Such computer will run not just agent process but also the master process. UPDATE: computer list not used, the configuration of each agent is local in local app config files.

#### Autodetection of the machine id

UPDATE: not used.
By comapring the computer's IP address with those available in the computer list the dirigent processes automaticaly determine on what machine they are running. There is no need to tell them what machine id they are going to use.


### Application boot up completion detection

Some apps take a long time to boot up and initialize. Dirigent should not start a dependent app until its dependecies are satisfied. By 'satisfied' it is meant that the all the dependencies are already running and that they have completed their initialization phase.

Dirigent supports multiple methods of detection whther an application is already up and running. The method can can be specified for each application in the launch plan.

The simplest methods do not require any involvement of the application - for example the time measured from app launch. Such method are usually suboptimal - they usually need to wait longer than abosolutely necessary to safely avoid premature completion. Better methods rely on some observable results of application execution - like showing a window, creating a file, creating a global mutext etc. For optimal results the application may be required to implement a direct support for such a detection.
 

#### Dirigent control

Dirigent can be controlled in multiple ways, each fitted for different use case. Everything can be controlled manually from the control GUI. Control commands can be sent to dirigent by executing its command line remote control application. Also a .net remote control library is available for embedding into user applications.
 

#### Execution of launch plan

A new launch plan automatically cancels any previous plan, i.e. all apps from the previous plan are killed. The application from the new plan are initially assigned the state 'not launched'.

The launch order of all apps form the plan is determined. The result is a sequence of so called launch waves. A wave contains applications whose depedencied have been satisfied by the previous launch wave. The first wave comprises apps that do not depend on enything else. In the next wawe there are apps dependent on the apps from the previous wave.

The waves are launched sequentially one after another until all apps from all waves have been launched. 

If some application fails to start, dirigent can be configured to retry the launch attempt multiple times.

If all attempts fail, the plaunch plan is stopped and an error is returned.

Pokud se n�kter� z aplikac� nepoda�� spustit, dirigent (v z�vislosti na nastaven� t� kter� aplikace) m��e pokus o spu�t�n� i n�kolik�t opakovat.

Skon��-li v�echny pokusy o spu�t�n� nezdarem, prov�d�n� spou�t�c�ho pl�nu se zastav� a nahl�s� se chyba. Chyba se hl�s� v�em ��astn�k�m. Zobrazit u�ivateli by ji m�l ale pouze zadavatel povelu pro spou�t�n� pl�nu. Pokud byl pl�n spu�t�n z dirigentova GUI, objev� se chyba v tomto GUI. Pokud o spu�t�n� pl�nu po��dala jin� aplikace (nap�. p�es s�), dostane chybovou zpr�vu zp�t po stejn�m kan�lu.



#### Message among agents and the master

Note: Master forwards an incoming message to all agents including the sender.

 - **start plan.** Master ��d� v�echny agenty o zah�jen� prov�d�n� dan�ho spou�t�c�ho pl�nu. Agenti rozhodnou o po�ad� aplikac� (vypo�tou vlny; ka�d� vych�z� ze stejn� sd�len� konfigurace) a za�nou spou�t�t aplikace. Pr�b�n� aktualizuj� stav aplikac� z pl�nu. Neskon��, dokud nejsou spu�t�n� v�echny nebo nedostanou povel k ukon�en� pl�nu. �sp�n� spu�t�n� v�eho nijak zvl᚝ nesignalizuje, master to pozn� s�m ze sd�len�ho stavu aplikac�.

 - **stop plan.** Master pos�l� agent�m povel k ukon�en� v�ech aplikac� dosud spu�t�n�ch v r�mci aktu�ln�ho pl�nu. Agenti aplikace pozab�j� a aktualizuj� sd�len� stav aplikac�.

 - **Stav lok�ln�ch aplikac�.** Agent pos�l� masterovi stav sv�ch lok�ln�ch aplikac�. Master si zaktualizuje stav aplikac� v repozit��i a roze�le stav v�ech aplikac� v�em agent�m.

 - **Stav v�ech aplikac�.** Master pos�l� v�em agent�m stav v�ech aplikac� tak, jak jej dostal od jednotliv�ch agent�. 

 - **Chyba spou�t�n� pl�nu.** Agent informuje mastera o selh�n� jeho ��sti spou�t�c�ho pl�nu.

 - **Restartuj zvolenou aplikaci.** Master pos�l� agentovi po�adavek na ukon�en� a n�sledn� spu�t�n� vybran� aplikace z konkr�tn�ho pl�nu.

 - **Ukon�i zvolenou aplikaci.** Master pos�l� agentovi po�adavek na ukon�en� aplikace. Agent aplikaci zabije a aktualizuje sd�len� stav aplikace.

 - **Spus� zvolenou aplikaci.** Master pos�l� agentovi po�adavek na spu�t�n� zvolen� aplikace z aktu�ln�ho pl�nu. Agent aplikaci spust� a aktualizuje sd�len� stav aplikace.

 - P�edej po�adavek masterovi. Agent ��d� mastera o proveden� akce, kterou potenci�ln� nezvl�dne s�m (nap�. spu�t�n� aplikace najin�m po��ta�i). Master po�adavek p�epo�le na agenta nebo agenty, kte�� po�adavek dok�� splnit.



 