pipeline{
    agent any
    triggers {
        pollSCM("* * * * *")
    }
    stages{
        stage("Build") {
            steps {
                sh "docker compose build"
            }
        }
    }
}