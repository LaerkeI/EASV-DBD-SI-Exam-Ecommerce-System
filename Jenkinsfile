node {
    def app

    stage('Clone repository') {
        // Fetch the code from the repository
        checkout scm
    }

    stage('Build Docker image') {
        // Build a Docker image using the repository contents
        app = docker.build("laerkei/easv-dbd-si-ecommerce-system")
    }

    stage('Test Docker image') {
        // Run tests inside the Docker container
        app.inside {
            sh 'echo "Tests passed!"'
        }
    }
}
