pipeline {
    agent any
    triggers {
        pollSCM('* * * * *') // Check for changes in the repository every minute
    }
    stages {
        stage('Build') {
            steps {
                script {
                    // Build the Docker images for all services
                    sh 'docker compose -f docker-compose.yml build'
                }
            }
        }