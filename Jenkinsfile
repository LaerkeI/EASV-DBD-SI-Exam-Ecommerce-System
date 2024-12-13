pipeline {
    agent any // Run on any available agent
    environment {
        IMAGE_NAME_ORDER = 'laerkeimeland/order-management-service'
        IMAGE_NAME_INVENTORY = 'laerkeimeland/inventory-management-service'
    }
    stages {
        stage('Clone Repository') {
            steps {
                // Checkout using GitHub credentials
                withCredentials([usernamePassword(credentialsId: 'GitHub', usernameVariable: 'GITHUB_USER', passwordVariable: 'GITHUB_TOKEN')]) {
                    script {
                        bat """
                        git config --global credential.helper wincred
                        IF EXIST EASV-DBD-SI-Ecommerce-System (
                            RMDIR /S /Q EASV-DBD-SI-Ecommerce-System
                        )
                        git clone https://${GITHUB_USER}:${GITHUB_TOKEN}@github.com/LaerkeI/EASV-DBD-SI-Ecommerce-System.git
                        """
                    }
                }
            }
        }
        stage('Build Docker Images') {
            steps {
                script {
                    // Build Docker images and tag them with both the build number and 'latest'
                    bat """
                    docker build -t ${env.IMAGE_NAME_ORDER}:${env.BUILD_NUMBER} ./order-management-service
                    docker tag ${env.IMAGE_NAME_ORDER}:${env.BUILD_NUMBER} ${env.IMAGE_NAME_ORDER}:latest
                    
                    docker build -t ${env.IMAGE_NAME_INVENTORY}:${env.BUILD_NUMBER} ./inventory-management-service
                    docker tag ${env.IMAGE_NAME_INVENTORY}:${env.BUILD_NUMBER} ${env.IMAGE_NAME_INVENTORY}:latest
                    """
                }
            }
        }
        stage('Push to DockerHub') {
            steps {
                withCredentials([usernamePassword(credentialsId: 'DockerHub', usernameVariable: 'DOCKERHUB_USR', passwordVariable: 'DOCKERHUB_PSW')]) {
                    script {
                        // Push both the versioned and 'latest' tags to DockerHub
                        bat """
                        docker login -u ${DOCKERHUB_USR} -p ${DOCKERHUB_PSW}
                        
                        docker push ${env.IMAGE_NAME_ORDER}:${env.BUILD_NUMBER}
                        docker push ${env.IMAGE_NAME_ORDER}:latest
                        
                        docker push ${env.IMAGE_NAME_INVENTORY}:${env.BUILD_NUMBER}
                        docker push ${env.IMAGE_NAME_INVENTORY}:latest
                        """
                    }
                }
            }
        }
        stage('Deploy') {
            steps {
                script {
                    // Set the TAG environment variable in Windows batch syntax
                    bat """
                    set TAG=${env.BUILD_NUMBER}
                    docker-compose down
                    docker-compose up -d
                    """
                }
            }
        }
    }
    post {
        // Clean after build
        always {
            cleanWs(cleanWhenNotBuilt: false,
                    deleteDirs: true,
                    disableDeferredWipeout: true,
                    notFailBuild: true,
                    patterns: [[pattern: '.gitignore', type: 'INCLUDE'],
                               [pattern: '.propsfile', type: 'EXCLUDE']])
        }
    }
}