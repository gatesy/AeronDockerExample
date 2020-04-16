### Build and run the media driver
* `docker build -t aeronmediadriver MediaDriver`
* `docker run --shm-size=512m --ipc=shareable --name aeronmediadriver aeronmediadriver`
Need a container name so that other containers can find it. Problem is, this creates a named container that you'll need to remove to be able to make again.
* `docker container rm aeronmediadriver` will remove the named container
* `docker container start/stop/restart aeronmediadriver` allow you to re-use a previously created media driver container.

Question: how do we tail the media driver log file?
* by some magic in here https://github.com/real-logic/aeron/wiki/Monitoring-and-Debugging#debug-logging 

### Build and run the publisher
* `docker build -t aeronpublisher -f Publisher/Dockerfile .`
* `docker run --ipc=container:aeronmediadriver -it aeronpublisher`
or specify a time period
* `docker run --ipc=container:aeronmediadriver -it aeronpublisher --period=00:00:01.5`
or specify an Aeron channel
* `docker run --ipc=container:aeronmediadriver -it aeronpublisher --channel 'aeron:udp?endpoint=224.0.1.1:40456'`

### Build and run the subscriber
* `docker build -t aeronsubscriber -f Subscriber/Dockerfile .`
* `docker run --ipc=container:aeronmediadriver -it aeronsubscriber`

### Generate code from SBE schema
Run 
```
java -Dsbe.target.language=uk.co.real_logic.sbe.generation.csharp.CSharp -Dsbe.csharp.generate.namespace.dir=false -Dsbe.output.dir=Sbe -jar sbe-all-1.17.0.jar protocol-v1.xml
```
in the Protocol directory.

Now:
* Many receivers - DONE
* Many publishers - DONE
* Multicast - DONE (locally)
* Upload to Git
* Cross box (laptop to workstation)