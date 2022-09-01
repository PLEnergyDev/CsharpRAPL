#!/bin/bash

comp=$(which javac) 

if [ "$comp" == "" ]; then
    echo "javac not found! Please install javac to continue" && exit 1
fi 

java=$("$2" --version) 

if [ "$java" == "" ]; then
        echo "java executable not found! Please add java to your path or provide a custom executable path" && exit 1
else
    echo "$java"
fi 


cd "$1" || exit 1

echo -n "Compiling... "
SUBSTRING=$(echo "$3"| cut -d'"' -f 2) || exit 1
if test "$SUBSTRING" != "" ; then
  echo -n "Compiler options: $SUBSTRING... "
  javac *.java "$SUBSTRING" || exit 1
else
  echo -n "No additional compiler options found... "
  javac *.java || exit 1
fi

jar -cmf MANIFEST.MF JavaBench.jar *.class || exit 1
printf "#!/bin/bash\n$2 -server -jar \"%s/JavaBench.jar\" \$1" "$1" > JavaBench.sh || exit 1
sudo chmod +x JavaBench.sh || exit 1 
echo "Done!"
exit 0
