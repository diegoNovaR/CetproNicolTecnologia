const CLAIM_ROLE = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
const CLAIM_EMAIL = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress';
const CLAIM_NAME_ID = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';

export interface DecodedToken {
  sub?: string;
  exp?: number;
  [claim: string]: unknown;
}

export function decodeJwt(token: string): DecodedToken | null {
  try {
    const payload = token.split('.')[1];
    const base64 = payload.replace(/-/g, '+').replace(/_/g, '/');
    const json = decodeURIComponent(
      atob(base64)
        .split('')
        .map((c) => '%' + c.charCodeAt(0).toString(16).padStart(2, '0'))
        .join('')
    );
    return JSON.parse(json);
  } catch {
    return null;
  }
}

export function getRoleFromToken(token: string): string | null {
  const decoded = decodeJwt(token);
  if (!decoded) return null;
  return (decoded[CLAIM_ROLE] as string) ?? (decoded['role'] as string) ?? null;
}

export function getEmailFromToken(token: string): string | null {
  const decoded = decodeJwt(token);
  if (!decoded) return null;
  return (decoded[CLAIM_EMAIL] as string) ?? (decoded['email'] as string) ?? null;
}

export function getUserIdFromToken(token: string): string | null {
  const decoded = decodeJwt(token);
  if (!decoded) return null;
  return (decoded['sub'] as string) ?? (decoded[CLAIM_NAME_ID] as string) ?? null;
}

export function isTokenExpired(token: string): boolean {
  const decoded = decodeJwt(token);
  if (!decoded?.exp) return true;
  return Date.now() >= decoded.exp * 1000;
}
