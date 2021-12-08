//1. create a pagination interface
export interface Pagination {
  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
}

//2. create a paginated result class type for the data returned from the server
export class PaginatedResult<T> {
  result: T;
  pagination: Pagination;
}
//3. next we use this result in the members.service.ts, the getMembers method. go to members.service.ts
