pipeline {
    agent { label 'vinod' }
    
    environment {
        DOCKER_IMAGE = 'dotnetapp'
        DOCKER_TAG = 'latest'
        REPOSITORY = 'onkar717/dotnet-hello-world'
        EC2_IP = '13.51.200.220'  // Replace with your EC2 instance IP
        EC2_USER = 'ec2-user'  // Replace with your EC2 user (could be 'ubuntu' or 'ec2-user')
    }

    stages {
        // Stage 1: Clone the code
        stage('Code') {
            steps {
                echo "Cloning the repository..."
                git url: 'https://github.com/onkar717/dotnet-hello-world.git', branch: 'main'
                echo "Code cloning successful"
            }
        }

        // Stage 2: Build the Docker image
        stage('Build') {
            steps {
                echo "Building the Docker image..."
                sh "docker build -t ${DOCKER_IMAGE}:${DOCKER_TAG} ."
            }
        }

        // Stage 3: Run Tests (Unit tests or other tests)
        stage('Test') {
            steps {
                echo "Running tests..."
                // Run your unit tests here. For example, for a .NET Core project:
                sh 'dotnet test'
            }
        }

        // Stage 4: Push to Docker Hub
        stage('Push to Docker Hub') {
            steps {
                echo "Pushing the image to Docker Hub..."
                withCredentials([usernamePassword(credentialsId: 'dockerHubCred', passwordVariable: 'dockerHubPass', usernameVariable: 'dockerHubUser')]) {
                    // Log in to Docker Hub
                    sh "docker login -u ${dockerHubUser} -p ${dockerHubPass}"
                    
                    // Tag the image with your Docker Hub username
                    sh "docker image tag ${DOCKER_IMAGE}:${DOCKER_TAG} ${dockerHubUser}/${DOCKER_IMAGE}:${DOCKER_TAG}"
                    
                    // Push the tagged image to Docker Hub
                    sh "docker push ${dockerHubUser}/${DOCKER_IMAGE}:${DOCKER_TAG}"
                }
            }
        }

        // Stage 5: Deploy using Docker Compose
        stage('Deploy') {
            steps {
                echo "Deploying the application using Docker Compose..."
                // Here you execute docker-compose commands to deploy the application
                sh 'docker-compose down && docker-compose up -d'
                echo "Deployment completed"
            }
        }

        // Stage 6: Health Check for the deployed application
        stage('Health Check') {
            steps {
                echo "Running health check..."
                // Verify if the application is up and running
                script {
                    def response = sh(script: "curl -s -o /dev/null -w '%{http_code}' http://${EC2_IP}:8080", returnStdout: true).trim()
                    if (response == "200") {
                        echo "Health check passed. Application is running."
                    } else {
                        error "Health check failed. Application is not running as expected."
                    }
                }
            }
        }
    }

    post {
        success {
            echo "Deployment completed successfully!"
        }
        failure {
            echo "Deployment failed!"
        }
    }
}
