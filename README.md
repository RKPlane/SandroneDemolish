# Empresa de Demolición con Patos

## Tienes que demoler edificios usando patos, los edificios tienen una masa dependiendo del tamaño, los patos tienen munición limitada, tienen una fuerza de lanzamiento y el objetivo es llegar a un límite de demolición para ganar

Cada edificio tiene un fixedjoint con determinados valores y una masa para que de la sensacion de que cada uno es diferente<br/>

DemolitionTracker se encarga de trackear cuantos edificios han sido demolidos, registra los bloques de la clase que se encarga de gestionarlos y decide cuando ganas con una formula.<br/>
GameManager se encarga del estado del juego con la información que recibe DemolitionTracker.<br/>
DuckLauncher es el que se encarga de que se lanza, con que fuerza, donde y la trayectoria de este.<br/>
Duck es la clase del pato donde añade una fuerza al colisionar para dar ese efecto de explosión alrededor de este.<br/>
StructureBlock es la clase que se encarga de enviar información a DemolitionTracker de que efectivamente se han demolido los edificios .<br/>
BuildingAnchor es una clase para los bloques de la base y dar esa sensacion de la base de los edificios<br/>
GameUI es el que gestiona la interfaz y la informacion que imprime esta en base a otros scripts<br/>

Lucia: Montado de nivel y Operaciones como fuerza, masa y cálculos para que la demolición se sienta bien<br/>
Luis: Menu e info printeado en la screen<br/>
Alberto: Code de las formulas<br/>
Sergi: Calculo de la fuerza y trayectoria Code<br/>

Video Gameplay: https://youtu.be/ByXBCKBQIOs
