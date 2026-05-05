# Empresa de Demolición con Patos

## Tienes que demoler edificios usando patos, los edificios tienen una masa dependiendo del tamaño, los patos tienen munición limitada, tienen una fuerza de lanzamiento y el objetivo es llegar a un límite de demolición para ganar

DemolitionTracker se encarga de trackear cuantos edificios han sido demolidos, registra los bloques de la clase que se encarga de gestionarlos y decide cuando ganas con una formula.
GameManager se encarga del estado del juego con la información que recibe DemolitionTracker.
DuckLauncher es el que se encarga de que se lanza, con que fuerza, donde y la trayectoria de este.
Duck es la clase del pato donde añade una fuerza al colisionar para dar ese efecto de explosión alrededor de este.
StructureBlock es la clase que se encarga de enviar información a DemolitionTracker de que efectivamente se han demolido los edificios.

\Lucia y Sergi: Montado de nivel y Operaciones como fuerza, masa y cálculos para que la demolición se sienta bien
\Luis: Menu e info printeado en la screen
Alberto: Main code
