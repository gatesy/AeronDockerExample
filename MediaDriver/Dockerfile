FROM openjdk
WORKDIR /aeron
COPY *.jar .
ENTRYPOINT [ "java", "-cp", "aeron-all-1.27.0.jar", "io.aeron.driver.MediaDriver" ]
