pipeline {
    agent none
    parameters {
        choice(name: 'ENVIRONMENT', choices: ['UAT', 'Production'], description: 'Choose the environment to deploy to')
    }
    stages {
        stage("Code") {
            agent { label params.ENVIRONMENT == 'UAT' ? 'uat-agent' : 'prod-agent' }
            steps {
                echo "This is Cloning"
                git url: "https://github.com/onkar717/dotnet-hello-world.git", branch: "main"
                echo "Code Cloning Successfully"
            }
        }
        stage("Build") {
            agent { label params.ENVIRONMENT == 'UAT' ? 'uat-agent' : 'prod-agent' }
            steps {
                echo "This is Building"
                sh "docker build -t dotnetapp:latest ."
            }
        }
        stage("Test") {
            agent { label params.ENVIRONMENT == 'UAT' ? 'uat-agent' : 'prod-agent' }
            steps {
                echo "This is Test"
            }
        }
        stage("Push to Docker Hub") {
            agent { label params.ENVIRONMENT == 'UAT' ? 'uat-agent' : 'prod-agent' }
            steps {
                echo "This is Pushing to Docker Hub"
                withCredentials([usernamePassword(credentialsId: "dockerHubCred", passwordVariable: "dockerHubPass", usernameVariable: "dockerHubUser")]) {
                    // Log in to Docker Hub
                    sh "docker login -u ${env.dockerHubUser} -p ${env.dockerHubPass}"
                    
                    // Tag the image with your Docker Hub username
                    sh "docker image tag dotnetapp:latest ${env.dockerHubUser}/dotnetapp:latest"
                    
                    // Push the tagged image to Docker Hub
                    sh "docker push ${env.dockerHubUser}/dotnetapp:latest"
                }
            }
        }
        stage("Deploy") {
            agent { label params.ENVIRONMENT == 'UAT' ? 'uat-agent' : 'prod-agent' }
            steps {
                echo "Deploying to ${params.ENVIRONMENT.toUpperCase()} environment..."
                sh """
                docker-compose down
                docker-compose up -d
                """
                echo "Deployment Completed for ${params.ENVIRONMENT.toUpperCase()}"
            }
        }
    }
}
