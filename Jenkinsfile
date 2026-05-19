pipeline {
    agent any
    stages {
        stage('Hello') {
            steps {
                echo 'Hello jenkins'
            }
        }
        stage('Build Docker Image') {
            steps {
                sh 'ls -l'
                sh 'docker compose build'
            }
        }
        stage('Start Docker Container') {
            steps {
                sh 'docker rm -f web-app-calculator-jenkins || true'
                sh 'docker compose up -d'
            }
        }
    }
}
