import { ClientMethod } from "../models/ClientMethod.model";

export class ClientMethodsService {
  clientMethods = [];

  clientMethodTimeoutMs;
  maxClientMethods;
  cleanUpIntervalMs;

  constructor(
    clientMethodTimeout = 60000,
    maxClientMethods = 200,
    cleanUpInterval = 5000
  ) {
    this.clientMethodTimeoutMs = clientMethodTimeout;
    this.maxClientMethods = maxClientMethods;
    this.cleanUpIntervalMs = cleanUpInterval;

    // cleanup loop
    setInterval(() => {
      this.clientMethods = [
        ...this.clientMethods.filter(
          (n) => !n.isExpired(this.clientMethodTimeoutMs)
        ),
      ];
    }, this.cleanUpIntervalMs);
  }

  clearAll() {
    this.clientMethods = [];
  }

  add(color, hubName, title, content) {
    const clientMethods = new ClientMethod(color, hubName, title, content);
    this.clientMethods = [
      clientMethods,
      ...this.clientMethods.slice(0, this.maxClientMethods - 1),
    ];
  }
}
