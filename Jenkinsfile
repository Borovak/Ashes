timestamps {

node () {

	stage ('Checkout') {
        checkout([$class: 'GitSCM', branches: [[name: '*/master']], doGenerateSubmoduleConfigurations: false, extensions: [], submoduleCfg: [], userRemoteConfigs: [[credentialsId: '3900d60a-c466-4a06-b15c-72dde9fea58f', url: 'https://github.com/Borovak/Ashes.git']]]) 
	}
	stage ('Deleting previous build output') {
        bat 'del /P "C:\\Users\\Public\\Documents\\AshesBuild\\*.*"'
    }
    stage ('Unity build') {
        bat '"C:\\Program Files\\Unity\\Hub\\Editor\\2020.2.7f1\\Editor\\Unity.exe" -projectPath "C:\\Program Files\\Jenkins\\workspace\\Ashes" -quit -batchmode -nographics -logFile Editor.log -executeMethod CommonFunctions.Editor.CommandLineBuild.BuildPlayer'
    }
    stage ('Inno package build') {
        bat 'rd /s /q "C:\\Users\\Public\\Documents\\AshesBuild\\Ashes_BackUpThisFolder_ButDontShipItWithYourGame"'
        bat '"C:\\Program Files (x86)\\Inno Setup 6\\iscc.exe" inno.iss /O"C:\\Users\\jenkins\\jenkins\\" /DApplicationVersion=0.1.%BUILD_NUMBER%' 
	}
	stage ('Moving package to NAS') {
	    bat 'move "C:\\Users\\jenkins\\jenkins\\Ashes 0.1.%BUILD_NUMBER%.exe" "\\\\boreli-nas\\Studio Boreli\\Releases\\Ashes\\Ashes 0.1.%BUILD_NUMBER%.exe"'
	}
	
}
}