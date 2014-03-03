# Dirigent
## Overview
Dirigent is a remote application management tool for a set of applications running on one or multiple networked computers. It runs on .net and Mono platforms, supporting both Windows and Linux operating systems.

It allows launching a given set of applications in given order according to predefined launch plan. The plan specifies what applications to launch, on what computers, in what order and whether to wait for a dependent apps to become fully initialized.

Applications can be remotely started, terminated or restarted, either individually or all-at-once as defined in current launch plan. 

The applications are continuously monitored whether they are already initialized and still running. Their status can be displayed on a control GUI. An application that is supposed to run continuously can be automatically restarted after crash.

The launch order is defined either by a fixed ordinal number assigned to an application or it is determined automatically based on the dependecies among applications.

Applications can be set to run on computer startup. To speedup the system startup process, certain applications can be launched even before the connection among computers is estabilished and - those apps that do not depend on other apps running on other computers.


## Architecture

### Agents and master

Each computer is running a background process - agent. One of the computers runs a master. Agents connect to master who is the central hub providing data interchange between agents.

Agents control the processes running locally - take care of local application launching, killing, restarting and status monitoring. 

Agents listens to and executes application management commands from master.

Agents publish the status of local applications to master who in turn spreads it to all other agents. The status include whether the app is running, whether it is already initialized etc.

Master is another background process managing the configuration of applications (launch plans etc.) The same configuration is shared among all agents.

### Control GUI
The control GUI is a standalone application connected to master.

Before manually executing a launch plan it can be quickly customized by removing some of the applications from the launch sequence.



#### Launch plan
Launch plan is just a list of apps to be launched in given order. Just one plan at a time is active.

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
Plan definition in an XML file uses so called templates allowing the inheritance of attributes. Every record in the plan can reference a template record. From the template all the attributes are loaded and only then they can ge overwritten by equally named attributes loaded from the referencing entry. The template record can reference another more generic template records.

#### Computer list
For each computer there is a textual machine id and the IP address defined. One of the machines is marked as master. Such computer will run not just agent process but also the master process.

#### Autodetection of the machine id
By comapring the computer's IP address with those available in the computer list the dirigent processes automaticaly determine on what machine they are running. There is no need to tell them what machine id they are going to use.


# Design notes in Czech
### Detekce dokon�en� inicializace aplikace
Jak dirigent pozn�, �e aplikace je ji� inicializov�na a �e m��eme za��st spou�t�t dal��, na n� z�visl� aplikace?

U ka�d� aplikace lze definovat pro ni specifick� mehcanismus. M��e to b�t:

 - podle �asu od spu�t�n�
 - podle glob�ln�ho synchroniza�n�ho objektu

Ka�d� lok�ln� dirigent distribuuje ostatn�m informace o inicializovanosti aplikac�. 

## Podpora v aplikac�ch
N�kter�m aplikac� trv� spou�t�n� a inicializace dlouhou dobu. U takov�ch aplikac� �ist� jen podle �asu od spu�t�n� nelze poznat, zda ji� je aplikace pln� funk�n� a �e se tedy mohou spou�t�t dal��, na n� z�visl�, aplikace). Aplikace m��e d�t najevo, �e je ji� inicializov�na, nap�. pomoc� glob�ln�ho synchroniza�n�ho objektu (nap�. mutexu). Tento je sledov�n lok�ln�m dirigentem.
 

    ## Ovl�d�n� dirigenta
Dirigenta lze ovl�dat n�kolika zp�soby, z nich� ka�d� se hod� pro jinou p��le�itost. V�echno lze d�lat interaktivn� z vestav�n�ho dirigentova GUI. Nebo lze d�vat dirigentovi povely spou�t�n�m jeho p��kazov�ho agenta a p�ed�n�m mu povel� na p��kazov� ��dce. Takt� se lze na mastera napojit po s�ti n�kter�m ze standardn�ch protokol� podporovan�ch ve WCF.
 
# Implementace
 
### Repozit�� stavu aplikac�
 
mapa appid na strukturu app state
 
App state obsahuje

  - aplikace b��, PID
  - aplikace ji� inicializov�na
  - odkaz na spou�t�c� pl�n
 
Konfigura�n� strom v pam�ov�, typov� bezpe�n� podob�

 - spou�t�c� pl�ny
 - seznam po��ta��
 

#### Aktualizace stavu aplikac�

Projedou se v�echny domn�le b��c� aplikace ze spou�t�c�ho pl�nu. Ov��� se, �e jejich PID st�le existuje. Prov��� se podm�nka inicializace (�as, mutex...)

#### Prov�d�n� spou�t�c�ho pl�nu

Nejprve dojde k ukon�en� aktu�ln�ho pl�nu, tedy k pozab�jen� aplikac�. Z nov�ho pl�nu se vyrob� polo�ky aplika�n�ho repozit��e ve stavu "nespu�t�no".

Vyhodnot� se po�ad� spou�ten� aplikac�. V�sledkem je seznam jenotliv�ch spou�t�c�ch "vln". Vlna obsahuje aplikace, jejich� z�vislosti ji� byly uspokojeny p�edchoz� vlnou. V prvn� vln� jsou aplikace, kter� nejsou z�visl� na ni�em. V druh� vln� aplikace z�visl� na t�ch z prvn� vlny. Ve t�et� jdou aplikace z�visl� na t�ch z druh� vlny a tak d�le.

Spou�t� se jedna vlna po druh�, dokud neb�� v�e. Dal�� vlna se v�ak spust� a� tehdy, pokud jsou spln�ny v�echny jej� podm�nky - tj. �e jsou ji� inicializovan� aplikace z p�edchoz� vlny.

Pokud se n�kter� z aplikac� nepoda�� spustit, dirigent (v z�vislosti na nastaven� t� kter� aplikace) m��e pokus o spu�t�n� i n�kolik�t opakovat.

Skon��-li v�echny pokusy o spu�t�n� nezdarem, prov�d�n� spou�t�c�ho pl�nu se zastav� a nahl�s� se chyba. Chyba se hl�s� zadavateli povelu pro spou�t�n� pl�nu. Pokud byl pl�n spu�t�n z dirigentova GUI, objev� se chyba v tomto GUI. Pokud o spu�t�n� pl�nu po��dala jin� aplikace (nap�. p�es s�), dotsane chybovou zpr�vu zp�t po stejn�m kan�lu.


#### Zpr�vy mezi masterem a agenty

Master komunikuje s agenty pomoc� zpr�v. Master se chov�, jako by v n�m b�el agent, a pos�l� ur�en� agent�m i s�m sob�.

 - **Prove� spou�t�c� pl�n.** Master ��d� v�echny agenty o zah�jen� prov�d�n� dan�ho spou�t�c�ho pl�nu. Agenti rozhodnou o po�ad� aplikac� (vypo�tou vlny; ka�d� vych�z� ze stejn� sd�len� konfigurace) a za�nou spou�t�t aplikace. Pr�b�n� aktualizuj� stav aplikac� z pl�nu. Neskon��, dokud nejsou spu�t�n� v�echny nebo nedostanou povel k ukon�en� pl�nu. �sp�n� spu�t�n� v�eho nijak zvl᚝ nesignalizuje, master to pozn� s�m ze sd�len�ho stavu aplikac�.

 - **Ukon�i aktu�ln� pl�n.** Master pos�l� agent�m povel k ukon�en� v�ech aplikac� dosud spu�t�n�ch v r�mci aktu�ln�ho pl�nu. Agenti aplikace pozab�j� a aktualizuj� sd�len� stav aplikac�.

 - **Stav lok�ln�ch aplikac�.** Agent pos�l� masterovi stav sv�ch lok�ln�ch aplikac�. Master si zaktualizuje stav aplikac� v repozit��i a roze�le stav v�ech aplikac� v�em agent�m.

 - **Stav v�ech aplikac�.** Master pos�l� v�em agent�m stav v�ech aplikac� tak, jak jej dostal od jednotliv�ch agent�. 

 - **Chyba spou�t�n� pl�nu.** Agent informuje mastera o selh�n� jeho ��sti spou�t�c�ho pl�nu.

 - **Restartuj zvolenou aplikaci.** Master pos�l� agentovi po�adavek na ukon�en� a n�sledn� spu�t�n� vybran� aplikace z konkr�tn�ho pl�nu.

 - **Ukon�i zvolenou aplikaci.** Master pos�l� agentovi po�adavek na ukon�en� aplikace. Agent aplikaci zabije a aktualizuje sd�len� stav aplikace.

 - **Spus� zvolenou aplikaci.** Master pos�l� agentovi po�adavek na spu�t�n� zvolen� aplikace z aktu�ln�ho pl�nu. Agent aplikaci spust� a aktualizuje sd�len� stav aplikace.

 - P�edej po�adavek masterovi. Agent ��d� mastera o proveden� akce, kterou potenci�ln� nezvl�dne s�m (nap�. spu�t�n� aplikace najin�m po��ta�i). Master po�adavek p�epo�le na agenta nebo agenty, kte�� po�adavek dok�� splnit.



 