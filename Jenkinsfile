pipeline {
    agent {
        docker {
            image 'docker:latest'
            args '-v /var/run/docker.sock:/var/run/docker.sock'
        }
    }
    environment {
        DOCKERHUB_CREDENTIALS = credentials('DockerHub')
        GITHUB_REPO = 'https://github.com/LaerkeI/EASV-DBD-SI-Ecommerce-System'
        IMAGE_NAME_ORDER = 'laerkeimeland/order-management-service'
        IMAGE_NAME_INVENTORY = 'laerkeimeland/inventory-management-service'
    }
    stages {
        stage('Clone Repository') {
            steps {
                checkout scm
            }
        }
        stage('Build Docker Images') {
            steps {
                script {
                    docker.build("${env.IMAGE_NAME_ORDER}:${env.BUILD_NUMBER}", "./OrderManagementService")
                    docker.build("${env.IMAGE_NAME_INVENTORY}:${env.BUILD_NUMBER}", "./InventoryManagementService")
                }
            }
        }
        stage('Test') {
            steps {
                script {
                    // Run tests for each service
                    docker.image("${env.IMAGE_NAME_ORDER}:${env.BUILD_NUMBER}").inside {
                        sh './run-tests.sh' // Replace with actual test script for Order Management
                    }
                    docker.image("${env.IMAGE_NAME_INVENTORY}:${env.BUILD_NUMBER}").inside {
                        sh './run-tests.sh' // Replace with actual test script for Inventory Management
                    }
                }
            }
        }
        stage('Push to DockerHub') {
            steps {
                script {
                    docker.withRegistry('https://index.docker.io/v1/', "${DOCKERHUB_CREDENTIALS}") {
                        docker.image("${env.IMAGE_NAME_ORDER}:${env.BUILD_NUMBER}").push()
                        docker.image("${env.IMAGE_NAME_INVENTORY}:${env.BUILD_NUMBER}").push()
                    }
                }
            }
        }
        stage('Deploy') {
            steps {
                script {
                    // Use docker-compose for deployment with dynamic tags
                    sh '''
                    TAG=${BUILD_NUMBER} docker-compose down
                    TAG=${BUILD_NUMBER} docker-compose up -d
                    '''
                }
            }
        }
    }
    post {
        success {
            echo 'Pipeline succeeded!'
        }
        failure {
            echo 'Pipeline failed.'
        }
    }
}
