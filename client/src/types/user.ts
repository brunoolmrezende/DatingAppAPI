export type User = {
  id: string;
  email: string;
  displayName: string;
  token: string;
  imageUrl?: string;
}

export type LoginCreds = {
  email: string;
  password: string;
}

export type RegisterCreds = {
  email: string;
  password: string;
  displayName: string;
}
