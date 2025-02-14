# Blackbird.io Phrase

Blackbird is the new automation backbone for the language technology industry. Blackbird provides enterprise-scale automation and orchestration with a simple no-code/low-code platform. Blackbird enables ambitious organizations to identify, vet and automate as many processes as possible. Not just localization workflows, but any business and IT process. This repository represents an application that is deployable on Blackbird and usable inside the workflow editor.

## Introduction

<!-- begin docs -->

Phrase (previously Memsource) is a software as a service platform designed to automate and streamline translating and localizing digital products, such as web or mobile apps, websites, marketing content, etc. for international markets.

## Before setting up

Before you can connect you need to make sure that:

- You have a Phrase account on the instance you want to connect to.
- You have permission to create and modify Registered OAuth apps.
- In Phrase go to Settings > Registered OAuth apps and click _New_.
- Fill in any name and description. For Redirect URI fill in: `https://bridge.blackbird.io/api/AuthorizationCode` and click _Save_.
- Copy the _Client ID_ of the newly created OAuth app.

## Connecting

1. Navigate to apps and search for Phrase. If you cannot find Phrase then click _Add App_ in the top right corner, select Phrase and add the app to your Blackbird environment.
2. Click _Add Connection_.
3. Name your connection for future reference e.g. 'My Phrase connection'.
4. Fill in the _Client ID_ that you copied from Phrase.
5. Select the Base URL to the Phrase instance you want to connect to base on the location of your Phrase's data center (the same you use to login to Phrase).
6. Click _Authorize connection_.
7. Follow the instructions that Phrase gives you, authorizing Blackbird.io to act on your behalf.
8. When you return to Blackbird, confirm that the connection has appeared and the status is _Connected_.

![1695994786183](image/README/1695994786183.png)

## Actions

### Analysis

- **List analyses**
- **Get analysis**
- **Create analysis**
- **Download analysis file**

### Clients

- **List clients**
- **Get client**
- **Add client**
- **Update client**
- **Delete client**

### Connector

- **List connector**
- **Get connector**

### Custom fields

- **Get date custom field value**
- **Get numeric custom field value**
- **Get single select custom field value**
- **Get multi select custom field value**
- **Get text custom field value**
- **Set date custom field value**
- **Set numeric custom field value**
- **Set single select custom field value**
- **Set text custom field value**

> If a Custom field is not present in a project, it will be added as part of the `Set` action.

### File

- **List all files**
- **Get file**
- **Upload file**

### Glossary

- **Create new glossary**
- **Export glossary**
- **Import glossary**

### Job

- **Search jobs**
- **Get job**
- **Create job**
- **Delete job**
- **Get segments**
- **Edit job**
- **Download target file**
- **Download bilingual file**
- **Upload bilingual file**
- **Pre-translate job**

### Pricelist

- **List price lists**

### Project

- **List projects**
- **List project templates**
- **Find project**
- **Get project**
- **Create project**
- **Create project from template**
- **Add target language**
- **Edit project**
- **Delete project**
- **Download project original files** 
- **Download project target files**
- **Assign providers from template**

### Project reference file

- **List reference files**
- **Create reference files**
- **Download reference file**
- **Delete reference file**

### Quality assurance

- **Add ignored warning**
- **List LQA profiles**
- **Delete LQA profile**
- **Run auto LQA**
- **Download LQA assessment**
- **Get LQA assessment**

### Quote

- **Get quote**
- **Create quote**
- **Delete quote**

### Translation

- **List translation settings**
- **Translate with MT**
- **Translate with MT by project**
- **Delete all translations**

### Translation memory

- **List translation memories**
- **Create translation memory**
- **Get translation memory**
- **Import TMX file**
- **Export translation memory**
- **Insert segment into memory**
- **Delete translation memory**

### User

- **List users**
- **Get user**
- **Find user**
- **Add user**
- **Update user**
- **Delete user**

### Vendor

- **Add vendor**
- **List vendors**
- **Get vendor**

## Events

### Project

- **On project created**
- **On project deleted**
- **On project due date changed**
- **On project metadata updated**
- **On shared project assigned**
- **On project status changed**
- **Find project termbase** get the termbase linked to a project based on optional filters

### Job

- **On job created**
- **On job deleted**
- **On continuous job updated**
- **On job assigned**
- **On job due date changed**
- **On job exported**
- **On job source updated**
- **On job status changed** If you are using checkpoints, please fill all optional inputs (including Project ID). It will work even without a specified Project ID, but using it will allow us to check if the job already has the specified status. The status "Completed by linguist" will also trigger the event if the optional status "Completed" is set.
- **On job target updated**
- **On job unexported**
- **On all jobs in workflow step reached status** Triggered when all jobs in a specific workflow step reach a specified status. Returns only jobs in the specified workflow step

### Template

- **On template created**
- **On template deleted**
- **On template updated**

### Analysis

- **On analysis created**

### Quality assurance

- **On LQA reports created** (Polling event)

## Example

![1695995715372](image/README/1695995715372.png)

This example shows one of many use cases. Here, whenever an article is published we fetch the missing translations and retrieve the article as an HTML file. Then we create a new Phrase project with the missing locales as the target languages and upload the article as jobs. We link the project with the article to automatically push the new translations back when they are done in a second bird.

## Analysis with other systems

Our app allows you to export an analysis file (via the **Download analysis file** action) and use it in your workflow whenever you need it. You can also specify the format in which you want to download the file (such as JSON, CSV, or log), and then import it into the service of your choice.

Here's an example to demonstrate this feature between the Phrase and XTRF apps:

![phrase-xtrf-1](image/README/phrase-xtrf-1.png)

![phrase-xtrf-2](image/README/phrase-xtrf-2.png)

![phrase-xtrf-2](image/README/phrase-xtrf-3.png)

## Missing features

Not all API endpoints are covered, let us know if you are missing features or if you see other improvements!

## Feedback

Feedback to our implementation of Phrase is always very welcome. Reach out to us using the [established channels](https://www.blackbird.io/) or create an issue.

<!-- end docs -->
