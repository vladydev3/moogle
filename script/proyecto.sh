#!/bin/bash

run_moogle(){
    cd ..
    make dev
}

show_report(){
    if [ ! -f "../informe/inf.pdf" ]; then
        pdflatex "../informe/inf.tex"
        echo "Creando inf.pdf..."
        xdg-usen ../informe/inf.pdf
        echo "Mostrando inf.pdf..."
    fi
}

show_slide(){
    if [ ! -f "../presentacion/pres.pdf"];then
        pdflatex "../presentacion/pres.tex"
        xdg-usen ../presentacion/pres.pdf
        echo "Mostrando pres.tex"
    fi
}

create_report(){
    pdflatex "../informe/inf.tex"
    echo "Archivo inf.pdf creado con éxito"
}

create_slide(){
    pdflatex "../presentacion/pres.tex"
    echo "Archivo pres.pdf creado con éxito."
}

clean_files(){   
   cd ../informe
   rm -f "*.aux" "*.lot" "*.lof" "*.log" "*.out" "*.gz" "*.toc" "*.fls" "*.fdb_latexmk"

   cd ../presentacion
   rm -f "*.aux" "*.log" "*.out" "*.synctex.gz" "*.toc" "*.fls" "*.fdb_latexmk" "*.vrb" "*.nav" "*.snm"

   echo "Archivos auxiliares borrados"
}

while  true 
do
    echo -e "Escriba un numero dependiendo de la opcion que desee: \n1-> run \n2-> report \n3-> slides \n4-> show_report \n5-> show_slides \n6-> clean \n7-> exit" 
    read us

    if [[ ! $us =~ ^[0-9]+$ ]];then
    echo -e "\n"

    elif [ $us -eq 1 ];then 
    run_moogle

    elif [ $us -eq 2 ];then
    create_report

    elif [ $us -eq 3 ];then
    create_slide

    elif [ $us -eq 4 ];then
    show_report

    elif [ $us -eq 5 ];then
    show_slide
  
    elif [ $us -eq 6 ];then
    clean_files

    elif [ $us -eq 7 ];then
    exit

    else
    echo -e "Seleccione un numero valido \n"
    fi
done
