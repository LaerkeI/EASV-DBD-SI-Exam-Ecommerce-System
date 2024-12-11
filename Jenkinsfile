pipeline {
    agent any // Run on any available agent
    environment {
        DOCKERHUB_CREDENTIALS = credentials('DockerHub')
        GITHUB_REPO = 'https://github.com/LaerkeI/EASV-DBD-SI-Ecommerce-System'
        IMAGE_NAME_ORDER = 'laerkeimeland/order-management-service'
        IMAGE_NAME_INVENTORY = 'laerkeimeland/inventory-management-service'
    }
    triggers {
        pollSCM("* * * * *") // Poll SCM for changes every minute
    }
    stages {
        stage('Clone Repository') {
            steps {
                checkout scm // Checkout code from the repository
            }
        }
        stage('Build Docker Images') {
            steps {
                script {
                    // Build Docker images and tag them with the build number
                    bat "docker-compose build --build-arg BUILD_NUMBER=${env.BUILD_NUMBER}"
                }
            }
        }
        stage('Push to DockerHub') {
            steps {
                script {
                    // Use --password-stdin to securely pass the password to docker login
                    bat """
                    echo ${DOCKERHUB_CREDENTIALS_PSW} | docker login -u ${DOCKERHUB_CREDENTIALS_USR} --password-stdin
                    docker push ${env.IMAGE_NAME_ORDER}:${env.BUILD_NUMBER}
                    docker push ${env.IMAGE_NAME_INVENTORY}:${env.BUILD_NUMBER}
                    """
                }
            }
        }
        stage('Deploy') {
            steps {
                script {
                    // Deploy using docker-compose with dynamic tags
                    bat """
                    TAG=${BUILD_NUMBER} docker-compose down
                    TAG=${BUILD_NUMBER} docker-compose up -d
                    """
                }
            }
        }
    }
    post {
        success {
            echo 'Pipeline succeeded!' // Output message if pipeline is successful
        }
        failure {
            echo 'Pipeline failed.' // Output message if pipeline fails
        }
    }
}
