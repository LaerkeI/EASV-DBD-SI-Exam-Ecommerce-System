# EASV-DBD-SI-Ecommerce-System

## Exam Notes

### RabbitMQ Messaging
The issue with `OrderEventConsumer` in `InventoryManagementService` not receiving messages from `OrderService` 
was because the consumer died (the consumer process stopped running) and no longer listened for incoming messages.

To solve this issue, the consumer should be implemented as a background service. Using a background service ensures 
that the consumer operates in its own dedicated thread and guarantees that the service continuously runs and listens 
for messages without being unintentionally terminated or interrupted by other parts of the application.

### Jenkins
- For this system, *Jenkins UI* is accessible at the default URL: `http://localhost:8080`.

- If there are any changes made to `Jenkinsfile`, the pipeline must first be run manually through *Jenkins UI*.
  Once this is done, the pipeline will subsequently run automatically for future changes.

- Credentials for *DockerHub* and *GitHub* need to be added in the *Jenkins UI* for the pipeline to function properly. 
  Both the DockerHub and GitHub credentials should use Personal Access Tokens (PAT) as the password.

- A GitHub webhook needs to be configured in the repository "EASV-DBD-SI-Ecommerce-System" on GitHub to enable automatic build triggering. 
  Use the build trigger `GitHub hook trigger for GITScm polling` when configuring the pipeline in *Jenkins UI*.
  When creating the webhook in GitHub, the `Payload URL` should be the public URL that forwards traffic to http://localhost:8080 through 
  ngrok, with `/github-webhook/` appended to the end. 
  
  For example:
  `https://<ngrok-generated-public-URL>/github-webhook/`

- Install the "Locale" plugin in Jenkins to force the Jenkins UI to display in English. Without this, the interface may display 
  a mix of English and poorly translated Danish.

### ngrok

*ngrok* is used as a reverse proxy to configure the GitHub webhook for *Jenkins* running on localhost.

**ngrok Setup**
- Sign up on the ngrok website and download the zip file from the "Setup & Installation" section.

- Unzip the file and run `ngrok.exe` to start the *ngrok Agent Command Line Interface (CLI)*.

- Paste the authentication token (found in your ngrok account dashboard right under the zip file) when prompted.

- Run the following command:
  `ngrok http http://localhost:8080`
  This will generate a random public URL that forwards traffic to http://localhost:8080.

- Important: If the ngrok account is on `Plan: Free`, a new random public URL is generated each time the ngrok http command is run.
  If the tunnel is closed or restarted, update the Payload URL in the GitHub webhook to the new public URL for the webhook to continue functioning.