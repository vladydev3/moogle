![](favicon.ico)
> Proyecto de Programación I.
> Facultad de Matemática y Computación - Universidad de La Habana.
> Cursos 2021, 2022.

Moogle! es una aplicación *totalmente original* de un modelo vectorial de recuperación de información cuyo propósito es buscar inteligentemente un texto en un conjunto de documentos.

Es una aplicación web, desarrollada con tecnología .NET Core 7.0, específicamente usando Blazor como *framework* web para la interfaz gráfica, y en el lenguaje C#.
La aplicación está dividida en dos componentes fundamentales:
- `MoogleServer` es un servidor web que renderiza la interfaz gráfica y sirve los resultados.
- `MoogleEngine` es una biblioteca de clases donde está implementada la lógica del algoritmo de búsqueda.
## Manual de Usuario:
1. Para ejecutar el proyecto descargue todos los archivos del repositorio y diríjase a través de la consola a la carpeta donde estos se encuentran.
2. Ejecute el comando `make dev` si se encuentra en Linux o `dotnet watch run --project MoogleServer` si se encuentra en Windows, luego de ejecutar el comando deberá esperar un tiempo en función de la cantidad de documentos que se vayan a cargar.
3. Luego le aparecerá la interfaz web y podrá realizar su consulta ingresando el texto deseado en la barra de búsqueda. Se le devolverán una serie de documentos en función de su petición.
4. Si su consulta no arroja resultados, se le dará una sugerencia con términos que si arrojen resultados dentro del corpus de documentos (esto siempre y cuando no se encuentren documentos que satisfagan ninguna de las palabras de la consulta).
5. Para enriquecer la búsqueda puede utilizar los siguientes operadores:
    
    - **Operador de “no presencia de la palabra” (!)**: Escriba el caracter '!' inmediatamente antes de la palabra sobre la que se va a ejecutar, sin un caracter en blanco por el medio Ej: '!perro' (Importante que se escriba correctamente). Esto garantiza que NO se muestre ningún documento en el que esté presente la palabra señalada.
    - **Operador de “presencia de la palabra” (∧)**: Escriba el caracter '∧' inmediatamente antes de la palabra sobre la que se va a ejecutar, sin un caracter en blanco por el medio Ej: '^perro'. Establece la obligatoriedad de que la palabra señalada esté presente en los documentos resultados.
    - **Operador de “cercanı́a” (∼)**: Escriba el caracter (∼)
inmediatamente después de la primera palabra sobre la que se va a ejecutar e inmediatamente
antes de la segunda palabra, sin un caracter en blanco por el medio Ej: 'perro~gato':
Calcula la cercanı́a de ambas palabras en los documentos (en los que aparezcan ambas a la
vez) y aumenta el score de dichos documentos, ordenados según la cercanı́a (mientras más
cercanas las palabras, más aumenta el score).

    - **Operador de “importancia” (*)**: Escriba el caracter '\*'
inmediatamente antes de la palabra sobre la que se va a ejecutar, sin un caracter en blanco por
el medio Ej: '\*palabra'. Esto modifica la importancia de una palabra en una búsqueda. Puede escribirse más de un caracter '\*' delante de la palabra, lo que denota
la cantidad de veces que se aumenta la importancia de la misma. (n caracteres '\*' delante
de una palabra, aumenta su importancia n veces). 


![](Interfaz.png)
