import { ResponseStatus } from "../enums/response-status";

export interface ApiResponse<T> {
  status: ResponseStatus;
  message: string;
  data: T | null;
}