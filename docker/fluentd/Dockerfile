# fluentd/Dockerfile

FROM fluent/fluentd:v1.17.1-debian-1.1
USER root
RUN gem uninstall -I elasticsearch && gem install elasticsearch -v 8.17.0
RUN ["gem", "install", "fluent-plugin-elasticsearch", "--no-document", "--version", "5.4.3"]
USER fluent
