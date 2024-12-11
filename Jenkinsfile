pipeline {
    agent any
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
                    sh 'docker-compose -f docker-compose.yml build'
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