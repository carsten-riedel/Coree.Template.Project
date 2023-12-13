#!/usr/bin/env bash
# sudo curl -sSL https://raw.githubusercontent.com/carsten-riedel/Coree.Template.Project/main/raw/dotnet.sh | bash
clear

createDirectoryIfNeeded() {
    local filepath="$1"
    local withsudo="$2"
    local dir="${filepath%/*}"

    # Check if the directory exists
    if [ ! -d "$dir" ]; then
        # Create the directory with sudo if withsudo is true
        if [ "$withsudo" = true ]; then
            sudo mkdir -p "$dir"
            echo "Directory created with sudo: $dir"
        else
            mkdir -p "$dir"
            echo "Directory created: $dir"
        fi
    else
        echo "Directory already exists: $dir"
    fi
}

addLineToFile() {
    local linetoadd="$1"
    local targetfile="$2"
    local withsudo="$3"

    if [ -z "$linetoadd" ]; then
        echo "No line provided to add."
        return 1
    fi

    if [ -z "$targetfile" ]; then
        echo "No target file specified."
        return 1
    fi

    if [ "$withsudo" != true ]; then
        if [ ! -w "$targetfile" ]; then
            echo "Target file is not writable without sudo."
        fi
    fi

    # Create directory if needed
    createDirectoryIfNeeded "$targetfile" "$withsudo"

    if [ ! -f "$targetfile" ] && [ "$withsudo" = true ]; then
            sudo touch "$targetfile"
        elif [ ! -f "$targetfile" ] && [ "$withsudo" != true ]; then
            touch "$targetfile"
        fi

    if ! grep -qF -- "$linetoadd" "$targetfile"; then

        if [ "$withsudo" = true ]; then
            echo "$linetoadd" | sudo tee -a "$targetfile" > /dev/null
        else
            echo "$linetoadd" | tee -a "$targetfile" > /dev/null
        fi

        echo "Line $linetoadd added to $targetfile"
    else
        echo "Line $linetoadd already present in $targetfile"
    fi
}

doesCommandExist() {
    command -v "$1" >/dev/null 2>&1
    if [ $? -eq 0 ]; then
        echo "true"
    else
        echo "false"
    fi
}


# Update the system
sudo apt-get -y update
#sudo apt-get -y -o APT::Get::Always-Include-Phased-Updates=true upgrade
sudo apt-get -y upgrade

sudo apt-get -y install mc

# https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-install-script
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 2.1
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 3.1
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 5.0
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 6.0
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 7.0
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 8.0

export DOTNET_ROOT=$HOME/.dotnet
export PATH=$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools
addLineToFile "export DOTNET_ROOT=$HOME/.dotnet" "$HOME/.bashrc"
addLineToFile "export PATH=\$PATH:\$DOTNET_ROOT:\$DOTNET_ROOT/tools" "$HOME/.bashrc"

dotnet tool install --global PowerShell 2>nul

result=$(doesCommandExist code)
if [ "$result" != "true" ]; then
    wget --no-clobber --content-disposition -O code.deb https://go.microsoft.com/fwlink/?LinkID=760868
    sudo apt install -y ./code.deb
    rm -f ./code.deb
    export DONT_PROMPT_WSL_INSTALL=1 ;
    addLineToFile "export DONT_PROMPT_WSL_INSTALL=1" "$HOME/.bashrc" 
fi

mkdir -p "$HOME/source/repos"
mkdir -p "$HOME/source/packages"

# Chrome
resultChrome=$(doesCommandExist google-chrome)
if [ "$resultChrome" != "true" ]; then
    wget https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb
    sudo apt -y install ./google-chrome-stable_current_amd64.deb
    rm -f ./google-chrome-stable_current_amd64.deb
    addLineToFile "alias chrome='setsid google-chrome &>/dev/null'" "~/.bash_aliases"
fi

resultNginx=$(doesCommandExist nginx)
if [ "$resultChrome" != "true" ]; then
    sudo apt-get install nginx
    sudo chmod -v -R 777 /var/www/html
fi

resultJenkins=$(doesCommandExist jenkins)
if [ "$resultJenkins" != "true" ]; then
    sudo wget -O /usr/share/keyrings/jenkins-keyring.asc https://pkg.jenkins.io/debian-stable/jenkins.io-2023.key
    echo deb [signed-by=/usr/share/keyrings/jenkins-keyring.asc] https://pkg.jenkins.io/debian-stable binary/ | sudo tee /etc/apt/sources.list.d/jenkins.list > /dev/null
    sudo apt-get update
    sudo apt-get install fontconfig openjdk-17-jre
    sudo apt-get install -y jenkins
    addLineToFile "[Service]" "/etc/systemd/system/jenkins.service.d/override.conf" true
    addLineToFile "Environment=\"JENKINS_PORT=8181\"" "/etc/systemd/system/jenkins.service.d/override.conf" true
    addLineToFile "Environment=\"JAVA_OPTS=-Djava.awt.headless=true -Djava.net.preferIPv4Stack=true -Djava.net.preferIPv4Addresses=true\"" "/etc/systemd/system/jenkins.service.d/override.conf" true
    sudo systemctl daemon-reload
    sudo systemctl restart jenkins
    echo "Jenkins initial password:" && sudo cat /var/lib/jenkins/secrets/initialAdminPassword
fi

#echo 'deb blah ... blah' | sudo tee -a /etc/apt/sources.list > /dev/null

#sudo wget -O /usr/share/keyrings/jenkins-keyring.asc https://pkg.jenkins.io/debian-stable/jenkins.io-2023.key
#echo deb [signed-by=/usr/share/keyrings/jenkins-keyring.asc] https://pkg.jenkins.io/debian-stable binary/ | sudo tee /etc/apt/sources.list.d/jenkins.list > /dev/null
#sudo apt-get update
#sudo apt install fontconfig openjdk-17-jre
#sudo apt-get install jenkins

#sudo systemctl edit jenkins /etc/systemd/system/jenkins.service.d/override.conf
#[Service]
#Environment="JENKINS_PORT=8181"
#Environment="JAVA_OPTS=-Djava.awt.headless=true -Djava.net.preferIPv4Stack=true -Djava.net.preferIPv4Addresses=true"
#check http://[::1]

#Shows the vEthernet(WSL) from windows
#ip route show | grep -i default | awk '{ print $3}'

#Shows the current wsl image ip
#hostname -I

#sudo service jenkins restart

