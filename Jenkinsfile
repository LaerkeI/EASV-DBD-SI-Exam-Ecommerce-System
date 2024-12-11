pipeline {
    agent {
        docker {
            image 'docker:latest'
            args '-v /var/run/docker.sock:/var/run/docker.sock'
        }
    }
    environment {
        DOCKERHUB_CREDENTIALS = credentials('DockerHub') // This can be used later for withCredentials block
        GITHUB_REPO = 'https://github.com/LaerkeI/EASV-DBD-SI-Ecommerce-System'
        IMAGE_NAME_ORDER = 'laerkeimeland/order-management-service'
        IMAGE_NAME_INVENTORY = 'laerkeimeland/inventory-management-service'
    }
    triggers {
        pollSCM("* * * * *")
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
                    sh "docker build -t ${env.IMAGE_NAME_ORDER}:${env.BUILD_NUMBER} ./OrderManagementService"
                    sh "docker build -t ${env.IMAGE_NAME_INVENTORY}:${env.BUILD_NUMBER} ./InventoryManagementService"
                }
            }
        }
        stage('Push to DockerHub') {
            steps {
                script {
                    // Using withCredentials for secure login
                    withCredentials([usernamePassword(credentialsId: 'DockerHub', usernameVariable: 'DOCKER_USERNAME', passwordVariable: 'DOCKER_PASSWORD')]) {
                        sh """
                        docker login -u ${DOCKER_USERNAME} -p ${DOCKER_PASSWORD}
                        docker push ${env.IMAGE_NAME_ORDER}:${env.BUILD_NUMBER}
                        docker push ${env.IMAGE_NAME_INVENTORY}:${env.BUILD_NUMBER}
                        """
                    }
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
