# Trabalho de Integração Continua – UECE
Igor Lucas Silva

## Introdução
Esse trabalho consiste na demonstração de um fluxo de integração continua em um projeto utilizando o .Net Core e o Jenkins. Esse projeto é uma simples aplicação de teste unitário que verifica se um número é primo ou não.

## Ferramenta utilizada
- Jenkins
- Visual Studio
- .Net Core

## Pipelines usados
- Limpeza do espaço de trabalho
- Git Checkout
- Restauração dos pacotes/dependências
- Limpeza do projeto para realizar o build
- Build, construção do artefato
- Execução de teste unitários

## Sobre o Jenkins
É um servidor de automação de código aberto independente que você pode usar para automatizar todos os tipos de tarefas relacionadas à construção, teste e entrega ou implantação de software. Ele trabalha executando automaticamente certos scripts para gerar arquivos necessários para implantação. Esses scripts são chamados JenkinsFiles, e são apenas arquivos de texto que podem conter código declarativo ou roteirizado.

## Requisitos básicos para rodar o projeto
- Java SDK
- Visual Studio
- Git

Instale o Jenkins de acordo com o seu ambiente [clicando aqui](https://www.jenkins.io/doc/book/installing/)

No momento da instalação, escolha a instalação de plugins sugeridos, como pode ser visto na imagem abaixo.

![image](https://user-images.githubusercontent.com/11475845/121449697-b1b6cf00-c970-11eb-9432-ff0c1af6199d.png)

Instale e configure o plugin MSBuilder no Jenkins, [link útil aqui](https://blog.couchbase.com/continuous-deployment-with-jenkins-and-net/#:~:text=MSBuild%20configuration%201%20Navigate%20to%20http%3A%2F%2Flocalhost%3A8080%2F%202%20Click,your%20system.Read%20below%20for%20...%209%20Click%20save.).

Após a conclusão da instalação, crie um novo job.

![image](https://user-images.githubusercontent.com/11475845/121449798-e460c780-c970-11eb-87ab-dc6c06a4cf91.png)

Esse job será usando para criar o pipeline executado no projeto, então o próximo passo é a configuração do pipeline.

![fGCtJNnhOZ](https://user-images.githubusercontent.com/11475845/121449897-14a86600-c971-11eb-99a2-1c7cfc7aba6e.gif)

Veja o video com a demonstração da execução do projeto [clicando aqui](https://youtu.be/V6AxXv6nYiU).

## Scrip do Pipeline
```
pipeline {
    agent any

    stages {
        stage ('Clean workspace') {
            steps {
                cleanWs()
            }
        }
        
        stage ('Git Checkout') {
            steps {
                git branch: 'main', credentialsId: '123-jenkins', url: 'https://github.com/igorlucas/demoGithubActions.git'
            }
        }
        
        stage('Restore packages') {
            steps {
                bat "msbuild /t:restore ${workspace}\\TestProject.sln"
            }
        }
        
        stage('Clean') {
            steps {
                bat "msbuild ${workspace}\\TestProject.sln /nologo /nr:false /p:platform=\"Any CPU\" /p:configuration=\"release\" /t:clean"
            }
        }
        
        stage('Build') {
            steps {
                bat "msbuild ${workspace}\\TestProject.sln /nologo /nr:false  /p:platform=\"Any CPU\" /p:configuration=\"release\" /t:clean;restore;rebuild"
            }
        }
        
        stage('Running unit tests') {
            steps {
                bat "dotnet test ${workspace}\\TestProject.sln"
            }        
        }
    }
} 
```

## Explicando os passos do pipeline
### 1. Limpa o diretorio onde os projetos serão construidos pelo jenkins
```
stage ('Clean workspace') {
            steps {
                cleanWs()
            }
        }
```
### 2. Clona o projeto de um repositório remoto e faz o checkout pra uma branch específica.
```
stage ('Git Checkout') {
            steps {
                git branch: 'main', credentialsId: '123-jenkins', url: 'https://github.com/igorlucas/demoGithubActions.git'
            }
        }
```
### 3. Restaura as dependências necessárias para o projeto
```
stage('Restore packages') {
            steps {
                bat "msbuild /t:restore ${workspace}\\TestProject.sln"
            }
        }
```
### 4. Limpa a solução e prepara para o build.
```
stage('Clean') {
            steps {
                bat "msbuild ${workspace}\\TestProject.sln /nologo /nr:false /p:platform=\"Any CPU\" /p:configuration=\"release\" /t:clean"
            }
        }
```
### 5. Build do projeto.
```
stage('Build') {
            steps {
                bat "msbuild ${workspace}\\TestProject.sln /nologo /nr:false  /p:platform=\"Any CPU\" /p:configuration=\"release\" /t:clean;restore;rebuild"
            }
        }
```
### 6. Executa os testes unitários.
```
stage('Running unit tests') {
            steps {
                bat "dotnet test ${workspace}\\TestProject.sln"
            }        
        }
```
