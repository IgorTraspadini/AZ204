# Creating a WEB static app using Azure Cloud Shell.

1. Dowload Azure CLI
2. Inside of the CMD console
   - az login --tenant TENANT_ID 
   - Inside your folder, clone the sample repo from https://github.com/Azure-Samples/html-docs-hello-world.git
3. Checking the resource groups avaliables
   - az group list --query "[].{id: name}" -o tsv
4. Creating and webapp in Azure
   - az webapp up -g RESOURCE_GROUP -n APPLICATION_NAME --html
   - g: resource group
   - n: application name
5. Checking the application
   - http://webappict.azurewebsites.net
6. List the resources
   - az resource list --query "[].{id:name}" -o tsv
8. Remove the webapp in Azure
   - az webapp stop --name <AppServiceName> --resource-group <ResourceGroupName> 
   - az webapp delete --name <AppServiceName> --resource-group <ResourceGroupName>
