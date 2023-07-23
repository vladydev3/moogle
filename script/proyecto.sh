#!/bin/bash

run_moogle(){
    cd ..
    make dev
}

show_report(){
    if [ ! -f "../informe/inf.pdf" ]; then
        pdflatex "../informe/inf.tex"
        echo "Creando inf.pdf..."
        xdg-open ../informe/inf.pdf
        echo "Mostrando inf.pdf..."
    fi
}

show_slide(){
    if [ ! -f "../presentacion/pres.pdf"];then
        pdflatex "../presentacion/pres.tex"
        xdg-open ../presentacion/pres.pdf
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

#clean_files()
#    
#    cd ../informe
#    rm -f "*.aux" "*.lot" "*.lof" "*.log" "*.out" "*.gz" "*.toc" "*.fls" "*.fdb_latexmk"
#
#    cd ./../presentacion
#    rm -f "slide.aux" "slide.log" "slide.out" "slide.synctex.gz" "slide.toc" "slide.fls" "slide.fdb_latexmk" "slide.vrb" "slide.nav" "slide.snm"
#
#    echo "Archivos auxiliares borrados"

while  true 
do
    echo -e "Escriba un numero dependiendo de la opcion que desee: \n1-> run \n2-> report \n3-> slides \n4-> show_report \n5-> show_slides \n6-> clean \n7-> exit" 
    read op

    if [[ ! $op =~ ^[0-9]+$ ]]
    then
    echo -e "\n"
    elif [ $op -eq 1 ] 
    then 
    run_moogle

    elif [ $op -eq 2 ]
    then
    create_report

    elif [ $op -eq 3 ]
    then
    create_slide

    elif [ $op -eq 4 ]
    then
    show_report

    elif [ $op -eq 5 ]
    then
    show_slide
  

    elif [ $op -eq 6 ]
    then
    echo "nada"

    elif [ $op -eq 7 ]
    then
    exit

    else
    echo -e "You did not select any of the given numbers \n"
    fi
done
