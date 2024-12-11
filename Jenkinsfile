pipeline {
    agent {
        docker { image 'node:22.12.0-alpine3.21' }
        label 'docker'
    }
    stages {
        stage('Checkout') {
            steps {
                git url: 'https://github.com/LaerkeI/EASV-DBD-SI-Ecommerce-System',
                    branch: 'jenkins',
                    credentialsId: 'Git'
            }
        }
       stage('Build') {
            steps {
                script {
                    // Build the Docker images for all services
                    sh 'docker-compose build'
                }
            }
        }
        stage('Test') {
            steps {
                echo 'Testing...'
            }
        }
        stage('Deploy') {
            steps {
                echo 'Deploying...'
            }
        }
        stage('Clean Workspace') {
            steps {
                cleanWs()
            }
        }
    }
}