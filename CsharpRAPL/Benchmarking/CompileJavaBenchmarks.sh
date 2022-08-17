#!/bin/bash

comp=$(which javac) 

if [ "$comp" == "" ]; then
    echo "javac not found! Please install javac to continue" && exit
fi 

echo -n "Compiling... "
SUBSTRING=$(echo "$2"| cut -d'"' -f 2)
if test "$SUBSTRING" != "" ; then
  echo -n "Compiler options: $SUBSTRING... "
else
  echo -n "No additional compiler options found... "
fi

javac *.java
jar -cmf MANIFEST.MF JavaBench.jar JavaRun/*.class -d out "$SUBSTRING"
echo "java -jar JavaBench.jar \$1" > JavaBench.sh

echo "Done!"
exit 