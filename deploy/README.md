# HOW TO DEPLOY DA.DinnerPlanner
1. copy *.service to `/etc/systemd/system`
2. run `createService.sh`
3. paste `nginx.snippet.conf` into `/etc/nginx/sites-enabled/default`
4. paste `appsettings.snippet.json` into `appsettings.json`customize Port and path