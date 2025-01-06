# EASV-DBD-SI-Ecommerce-System

test for demo

## Til DBD-delen
- Lav transaktioner (Læs async design afsnittet i Art of Scalability i W37 DLS)
- Optimér database (indexing m.m. - Tjek chatgpts foreslag). Brug Patricks benchmarking tool til at teste. 

## Common errors
`An exception has been raised that is likely due to a transient failure. Consider enabling transient error resiliency by adding 'EnableRetryOnFailure' to the 'UseSqlServer' call`
= The database is not up yet. Give it a moment and try again. That usually solves the problem. 

`A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections. (provider: TCP Provider, error: 0 - No such host is known.)`
= The server name in the connection string in appsettings.json needs to be `localhost, <port-mapped-to-on-host>` and not `<service-name>, <port-mapped-to-on-host>`

## Exam Notes


### RabbitMQ Messaging
The issue with `OrderEventConsumer` in `InventoryManagementService` not receiving messages from `OrderService` 
was because the consumer died (the consumer process stopped running) and no longer listened for incoming messages.

To solve this issue, the consumer should be implemented as a background service. Using a background service ensures 
that the consumer operates in its own dedicated thread and guarantees that the service continuously runs and listens 
for messages without being unintentionally terminated or interrupted by other parts of the application.

#### Messaging in the Service layer (Separation of Concerns)
The controller should handle HTTP requests and responses, delegating the core business logic to the service layer.
Messaging is part of the business logic and is closely tied to the domain (e.g., publishing events about order creation or updates). This logic belongs in the service layer.


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


### Kubernetes

Pods for a single container. You don't need to create pods directly and should instead use Deployment. 
Services for load balancing
Replica Sets for spinning up more instances of the same container (pod?)


NodePort in Service:
Exposes the service externally.
Binds the service to a port on the Docker Desktop node, allowing access via localhost:<NodePort>.
Traffic from outside the cluster is not directly load-balanced; it is routed to a node, which then load-balances the traffic across the pods.

**Setup: Kubernetes Dashboard**

First apply the following YAML file to create the Kubernetes Dashboard:

`kubectl apply -f https://raw.githubusercontent.com/kubernetes/dashboard/v2.7.0/aio/deploy/recommended.yaml`

Create service account and grant privileges:

`kubectl create sa webadmin -n kubernetes-dashboard` 

`kubectl create clusterrolebinding webadmin --clusterrole=cluster-admin --serviceaccount=kubernetes-dashboard:webadmin`

Get the token for the service account:

`kubectl create token webadmin -n kubernetes-dashboard`

Run the following command:

`kubectl proxy`

Open the following URL in your browser:

http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/

Use the token to login.
