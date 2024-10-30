dashboard# dashboard

Create docker hub repository - publish
```
docker build -t dashboard-api . 
docker run -it --rm -p 5001:80 --name dashboard_container dashboard-api
docker run -d --restart=always --name dashboard_container -p 5001:80 dashboard-api
docker run -d --restart=always -v d:/volumes/dashboard/images:/app/wwwroot/images -v d:/volumes/dashboard/templates:/app/wwwroot/templates --name dashboard_container -p 5001:80 dashboard-api
 
docker ps -a
docker stop dashboard_container
docker rm dashboard_container

docker images --all
docker rmi dashboard-api

docker login
docker tag dashboard-front:latest potapchuk22/dashboard-front:latest
docker push potapchuk22/dashboard-front:latest

docker pull potapchuk22/dashboard-api:latest
docker ps -a
docker run -d --restart=always --name dashboard_container -p 5001:80 potapchuk22/dashboard-api

docker run -d --restart=always -v /volumes/dashboard/images/users:/app/wwwroot/images/users -v /volumes/dashboard/templates:/app/wwwroot/templates --name dashboard_container -p 5001:80 potapchuk22/dashboard-api


docker pull potapchuk22/dashboard-api:latest
docker images --all
docker ps -a
docker stop dashboard_container
docker rm dashboard_container
docker run -d --restart=always --name dashboard_container -p 5001:80 potapchuk22/dashboard-api
```