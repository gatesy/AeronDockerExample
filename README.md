# Aeron Docker Example
This is a suite of programs and Docker containers that allow you to run and test an Aeron middleware system inside Docker containers. The key thing is that the media driver lives in a different container than the apps that use it.

All commands are run from the root directory.

### Build and run the media driver
Build the _aeronmediadriver_ container.
```
docker build -t aeronmediadriver MediaDriver
```
Run the media driver container with 512MB of shared memory (the default is 64MB).
```
docker run --shm-size=512m --ipc=shareable --name aeronmediadriver aeronmediadriver
```
You need a container name so that other containers can find it. Problem is, this creates a named container that you'll need to remove to be able to make again.
* `docker container rm aeronmediadriver` will remove the named container
* `docker container start/stop/restart aeronmediadriver` allow you to re-use a previously created media driver container.

### Build and run the publisher
Build the publisher app
```
docker build -t aeronpublisher -f Publisher/Dockerfile .
```
Run is with the default settings
```
docker run --ipc=container:aeronmediadriver -it aeronpublisher
```
or specify a time period
```
docker run --ipc=container:aeronmediadriver -it aeronpublisher --period=00:00:01.5
```
or specify an Aeron channel
```
docker run --ipc=container:aeronmediadriver -it aeronpublisher --channel 'aeron:udp?endpoint=224.0.1.1:40456'
```

### Build and run the subscriber
Build the subscriber app
```
docker build -t aeronsubscriber -f Subscriber/Dockerfile .
```
Run it with the default settings
```
docker run --ipc=container:aeronmediadriver -it aeronsubscriber
```
You can also specify the Aeron channel with `--channel` and the Aeron stream ID with `--stream-id`.

### Generate code from SBE schema
In the `Protocol` library's directory run:
```
java \
    -Dsbe.target.language=uk.co.real_logic.sbe.generation.csharp.CSharp \
    -Dsbe.csharp.generate.namespace.dir=false \
    -Dsbe.output.dir=Sbe \
    -jar sbe-all-1.17.0.jar protocol-v1.xml
```

Now:
* Many receivers - DONE
* Many publishers - DONE
* Multicast - DONE (locally)
* Upload to Git - DONE
* Cross box (laptop to workstation)

### Run all the services using `docker-compose`
In the project directory run the command:
```
docker-compose up
```
This will spark up a service that contains an Aeron media driver, two publishers and a subscriber. Stop it using Ctrl+C, or pass the `-d` flag in and then run
```
docker-compose down
```
To shut everything down.