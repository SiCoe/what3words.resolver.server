# what3words.resolver.server

## Development

### Setup

For calling out to the what3words api you'll need to [request an api key](http://developer.what3words.com/api-register/)

Once you have this key, create a file called `w3wconfiguration.<EnvironmentName>.json` under your *what3words-resolver-server.Api* directory.
Where `<EnvironmentName>` is your current `env.EnvironmentName` (see Startup.cs), likely to be "Development"

This file should contain the following:

```json`
{
  "What3Words": {
    "w3wApiKey": "<YOUR API KEY>"
  }  
}
```

Where `<YOUR API KEY>` looks something like *1A2BC345*