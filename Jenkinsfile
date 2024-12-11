pipeline {
    agent any // Run on any available agent
    environment {
        DOCKERHUB_CREDENTIALS = credentials('DockerHub') // Credentials for DockerHub
        GITHUB_CREDENTIALS = credentials('GitHub') // Credentials for GitHub
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
                    // Build Docker images and tag them with the build number
                    bat """
                    docker-compose build --build-arg BUILD_NUMBER=${env.BUILD_NUMBER}
                    """
                }
            }
        }
        stage('Push to DockerHub') {
            steps {
                withCredentials([usernamePassword(credentialsId: 'DockerHub', passwordVariable: 'DOCKERHUB_PSW', usernameVariable: 'DOCKERHUB_USR')]) {
                    script {
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