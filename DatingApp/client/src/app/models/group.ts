
//1. implement the group interface and the connection interface.
export interface Group {
  name: string;
  connections: Connection[];
}


interface Connection {
  connectionId: string;
  username: string;
}

//2. we'll make use of this in the message service, go to message.service.ts
