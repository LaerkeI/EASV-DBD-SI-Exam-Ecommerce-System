pipeline {
    agent any // Run on any available agent
    environment {
        DOCKERHUB_CREDENTIALS = credentials('DockerHub') // Credentials for DockerHub
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
                    bat """
                    docker-compose build --build-arg BUILD_NUMBER=${env.BUILD_NUMBER}
                    """
                }
            }
        }
        stage('Test') {
            steps {
                script {
                    // Run tests for each service in the Docker containers
                    bat "docker run --rm ${env.IMAGE_NAME_ORDER}:${env.BUILD_NUMBER} ./run-tests.sh"
                    bat "docker run --rm ${env.IMAGE_NAME_INVENTORY}:${env.BUILD_NUMBER} ./run-tests.sh"
                }
            }
        }
        stage('Push to DockerHub') {
            steps {
                withCredentials([usernamePassword(credentialsId: 'DockerHub', passwordVariable: 'DOCKERHUB_PSW', usernameVariable: 'DOCKERHUB_USR')]) {
                    script {
                        // Use --password-stdin to securely pass the password to docker login
                        bat """
                        echo ${DOCKERHUB_PSW} | docker login -u ${DOCKERHUB_USR} --password-stdin
                        docker push ${env.IMAGE_NAME_ORDER}:${env.BUILD_NUMBER}
                        docker push ${env.IMAGE_NAME_INVENTORY}:${env.BUILD_NUMBER}
                        """
                    }
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
